using OpenTK.Graphics.OpenGL;
using Simple_Engine.Engine.Core.Abstracts;

namespace Simple_Engine.Engine.Render.ShaderSystem
{
    public enum GeometryType
    {
        Lines,
        Points
    }

    public class Geo_Shader : Base_Shader
    {
        public int Location_SelectedVertex0 = -1;
        public int Location_SelectedVertex1 = -1;
        public int Location_SelectedVertex2 = -1;

        public GeometryType GeoType { get; }

        public Geo_Shader(GeometryType geoType, ShaderPath shaderModelType) : base(shaderModelType)
        {
            GeoType = geoType;
            InitalizeShader();
        }

        public override void Setup_Shader(string vertexPath, string fragmentPath)
        {
            //Link both Shaders to a program
            ProgramId = GL.CreateProgram();

            var vertexShaderId = CreateShader(vertexPath, OpenTK.Graphics.OpenGL.ShaderType.VertexShader);
            var geoShaderId = -1;
            switch (GeoType)
            {
                case GeometryType.Lines:
                    geoShaderId = CreateShader(LinesShader, OpenTK.Graphics.OpenGL.ShaderType.GeometryShader);
                    break;

                case GeometryType.Points:
                    geoShaderId = CreateShader(PointsShader, OpenTK.Graphics.OpenGL.ShaderType.GeometryShader);
                    break;
            }

            var fragShaderId = CreateShader(fragmentPath, OpenTK.Graphics.OpenGL.ShaderType.FragmentShader);

            AttachShader(ProgramId, vertexShaderId);
            AttachShader(ProgramId, geoShaderId);
            AttachShader(ProgramId, fragShaderId);

            BindVertexAttributes(); //must be before linking program

            LinkProgram(ProgramId);

            //After linking(whether successfully or not), it is a good idea to detach all shader objects from the program.
            GL.DetachShader(ProgramId, vertexShaderId);
            GL.DetachShader(ProgramId, geoShaderId);
            GL.DetachShader(ProgramId, fragShaderId);
            GL.DeleteShader(vertexShaderId);
            GL.DeleteShader(geoShaderId);
            GL.DeleteShader(fragShaderId);

            LoadAllUniforms();
            Use();
            SetVector4(Location_DefaultColor, new OpenTK.Vector4(0, 0, 1, 1));
            Stop();
        }

        public override void LoadAllUniforms()
        {
            base.LoadAllUniforms();
            Location_SelectedVertex0 = GetLocation("SelectedVertex0");
            Location_SelectedVertex1 = GetLocation("SelectedVertex1");
            Location_SelectedVertex2 = GetLocation("SelectedVertex2");
        }
    }
}