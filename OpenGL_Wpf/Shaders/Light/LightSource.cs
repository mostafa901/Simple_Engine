using OpenGL_CSharp.Geometery;
using OpenGL_CSharp.Graphic;
using OpenGL_Wpf;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MVVM;

namespace OpenGL_CSharp.Shaders.Light
{
	public class LightSource : BaseGeometry
	{
		public Vector3 ambient = new Vector3(.2f);

		public Vector3 specular = new Vector3(.8f);

		public Vector3 Direction = new Vector3(1,0,0);
		public float OuterAngle = 0;
		public float InnerAngle = 0;

		public int LightType;  //0=>Point, 1=>Direction 2=>Spot Light

		public float Constance = 1;
		public float Linear = 0.09f;
		public float Quaderic = .032f; //value used for attenuation calculations


		#region Attenuate

		private bool _Attenuate;

		public bool Attenuate
		{
			get
			{
				return _Attenuate;
			}
			set { SetProperty(ref _Attenuate, value); }

		}
		#endregion


		#region CMD_Attenuate

		private cus_CMD _CMD_Attenuate;

		public cus_CMD CMD_Attenuate
		{
			get
			{
				if (_CMD_Attenuate == null) _CMD_Attenuate = new cus_CMD();
				return _CMD_Attenuate;
			}
			set { SetProperty(ref _CMD_Attenuate, value); }

		}
		#endregion


		#region LightPosition

		private Vertex3 _LightPosition=new Vertex3(0,0,0);

		public Vertex3 LightPosition
		{
			get
			{
				return _LightPosition;
			}
			set { SetProperty(ref _LightPosition, value); }

		}
		#endregion


		#region Diffuse

		private Vertex3 _Diffuse;

		public Vertex3 Diffuse
		{
			get
			{
				return _Diffuse;
			}
			set { SetProperty(ref _Diffuse, value); }

		}
		#endregion

		#region SwitchLight

		private cus_CMD _SwitchLight;

		public cus_CMD CMD_SwitchLight
		{
			get
			{
				if (_SwitchLight == null) _SwitchLight = new cus_CMD();
				return _SwitchLight;
			}
			set { SetProperty(ref _SwitchLight, value); }

		}
		#endregion

		#region OnOff

		private bool _OnOff = true;
		Vertex3 OldDiffuse;
		Vector3 OldSpecular;
		public bool OnOff
		{
			get
			{
				return _OnOff;
			}
			set
			{
				SetProperty(ref _OnOff, value);
			}

		}
		#endregion

		public LightSource()
		{
			objectColor = new Vector3(1, 1, 0);
			OldDiffuse = Diffuse;
			OldSpecular = specular;			

			SetupActions();
		}

		private void SetupActions()
		{
			CMD_SwitchLight.Action = (a) =>
			{
				OnOff = (bool)a;
				if (!OnOff)
				{
					Diffuse.Update(new Vector3(0));
					specular = Diffuse.vector3;
				}
				else
				{
					Diffuse = OldDiffuse;
					specular = OldSpecular;
				}
			};

			LightPosition.PropertyChanged += delegate
			{
				Direction = LightPosition.vector3 + 1 * Direction.Normalized();
			};

			CMD_Attenuate.Action = (a) =>
			{
				Attenuate = (bool)a;
			};
		}

		public static List<LightSource> SetupLights(int count)
		{
			var lsources = new List<LightSource>();

			for (int i = 0; i < count; i++)
			{
				var l = new LightSource();
				l.Name = $"Light{i.ToString("00")}";
				l.primitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Lines;

				l.LightType = 0;
				//l.Direction = -Vector3.UnitY;
				l.LightPosition.Update(new Vector3(2, 2f, 0));
				l.Diffuse = Vertex3.FromVertex3(new Vector3(1));

				l.InnerAngle = 12.5f;
				l.OuterAngle = 0;
				lsources.Add(l);
			}
#if false
			//sky light
			lsources[1].diffuse = new Vector3(0, .01f, .2f);
			lsources[1].specular = new Vector3(0, .01f, .2f);
			lsources[1].ambient = new Vector3(0, .01f, .2f);
#endif
			return lsources;
		}

		public override void LoadGeometry()
		{
			Indeces = new List<int> { 0, 1 };
			var vcol = new Vertex4(objectColor.X, objectColor.Y, objectColor.Z, 1);
			points.Clear();
			points.Add(new Graphic.Vertex()
			{
				Position = LightPosition,
				TexCoor = new Vertex2(0, 0),
				Normal = Vertex3.FromVertex3(Direction),
				Vcolor = vcol
			});
			points.Add(new Graphic.Vertex()
			{
				Position = Vertex3.FromVertex3(Direction),
				TexCoor = new Vertex2(0, 0),
				Normal = Vertex3.FromVertex3(Direction),
				Vcolor = vcol
			});
			primitiveType = PrimitiveType.Lines;

			vers = null; //set this to null to update vertex information.						
			base.LoadGeometry();
			shader = new ObjectColor();

		}
	}
}
