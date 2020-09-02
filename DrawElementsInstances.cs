/// this is only for discussion purposes

 private void StoreDataInAttributeList(int location, float[] data, int componentCount)
        {
            var VBO = GL.GenBuffer(); //Create an Id for the Vertex Buffer Object
            VBOs.Add(VBO);
            //define the type of buffer in the GPU and Activate
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            //Supply the data to the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length, data, BufferUsageHint.StaticDraw);
             
            //Define the Pattern how the data is being read
            GL.VertexAttribPointer(
                location, //location layout --> see shader.vert
                componentCount, //vertex component count
                VertexAttribPointerType.Float, //type of vertex
                false, //shall normalize
                componentCount * sizeof(float), //the stride length
                0 //Start reading from What position within the stride
                );
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

private void StoreDataInAttributeList(int location)
        {
            var VBO = GL.GenBuffer(); //Create an Id for the Vertex Buffer Object
            VBOs.Add(VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);  //define the type of buffer in the GPU
            var transforms = Geo.Meshes.Select(o => o.Transform).ToArray();

            //now stream these vertex (array type) to the located buffer in the GPU
            GL.BufferData(BufferTarget.ArrayBuffer, Geo.Meshes.Count * sizeof(float) * 16, transforms, BufferUsageHint.StaticDraw);
            ErrorCheck();

            int componentCount = 4;

            for (int i = 0; i < 4; i++)
            {
                var attributeLocation = location + i;
                
                GL.VertexAttribPointer
                                        (
                                        attributeLocation,
                                        componentCount, //maximum is 4
                                        VertexAttribPointerType.Float,
                                        false,
                                        sizeof(float) * 16, //total matrix float Size
                                        sizeof(float) * i * componentCount //start reading from
                                        );

                MatrixLocations.Add(attributeLocation);
                GL.VertexAttribDivisor(attributeLocation, 1);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        }
public void Draw()
        {
			      Geo.ShaderModel.Use();
            GL.BindVertexArray(VAO);
            GL.EnableVertexAttribArray(PositionLocation);//position
            GL.EnableVertexAttribArray(TextureLocation);//texture
            GL.EnableVertexAttribArray(NormalLocation);//normal
            for (int i = 0; i < MatrixLocations.Count; i++)
            {
                GL.EnableVertexAttribArray(MatrixLocations[i]); //Matrix Rows
            } 
			
            Geo.ShaderModel.SetMatrix4(Geo.ShaderModel.ViewTransformLocation, Game.Instance.ActiveCamera.ViewTransform);
            Geo.ShaderModel.SetMatrix4(Geo.ShaderModel.ProjectionTransformLocation, Game.Instance.ActiveCamera.ProjectionTransform);
                         
            ErrorCheck();
 
            GL.DrawElementsInstanced(Geo.DrawType, Geo.Indeces.Count, DrawElementsType.UnsignedInt, Geo.Indeces.ToArray(), Geo.Meshes.Count);
               
            GL.DisableVertexAttribArray(PositionLocation);
            GL.DisableVertexAttribArray(TextureLocation);
            GL.DisableVertexAttribArray(NormalLocation);
            for (int i = 0; i < MatrixLocations.Count; i++)
            {
                GL.DisableVertexAttribArray(MatrixLocations[i]);//Instances
            }
            GL.BindVertexArray(0);
             
			      Geo.ShaderModel.Stop();
        }
