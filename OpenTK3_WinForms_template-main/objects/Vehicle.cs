using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK3_StandardTemplate_WinForms.objects
{
    internal class Vehicle
    {
        private struct FaceVertex
        {
            public int VertexIndex;
            public int TextureIndex;
            public int NormalIndex;
        }

        private List<Vector3> vertices;
        private List<Vector3> normals;
        private List<Vector2> textureCoords;
        private List<FaceVertex[]> faces;

        private Color color;
        private int textureId;
        
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public float Size { get; set; } = 0.2f;

        public Vehicle(string objPath, Color color)
        {
            this.color = color;
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            textureCoords = new List<Vector2>();
            faces = new List<FaceVertex[]>();

            LoadObj(objPath);

            this.Position = new Vector3(0, 0, 50);
        }

        public void LoadTexture(string path)
        {
             if (!File.Exists(path))
            {
                throw new FileNotFoundException("Texture file not found", path);
            }

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(path);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            textureId = id;
        }

        private void LoadObj(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("OBJ file not found", path);
            }

            foreach (var line in File.ReadLines(path))
            {
                string cleanLine = line.Trim();
                if (cleanLine.StartsWith("#") || string.IsNullOrEmpty(cleanLine)) continue;

                var parts = cleanLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                switch (parts[0])
                {
                    case "v":
                        if (parts.Length >= 4)
                        {
                            if (float.TryParse(parts[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float x) &&
                                float.TryParse(parts[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float y) &&
                                float.TryParse(parts[3], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float z))
                            {
                                vertices.Add(new Vector3(x, y, z));
                            }
                        }
                        break;
                    case "vn":
                        if (parts.Length >= 4)
                        {
                            if (float.TryParse(parts[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float nx) &&
                                float.TryParse(parts[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float ny) &&
                                float.TryParse(parts[3], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float nz))
                            {
                                normals.Add(new Vector3(nx, ny, nz));
                            }
                        }
                        break;
                    case "vt":
                        if (parts.Length >= 3)
                        {
                             if (float.TryParse(parts[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float u) &&
                                 float.TryParse(parts[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float v))
                             {
                                 textureCoords.Add(new Vector2(u, v));
                             }
                        }
                        break;
                    case "f":
                        FaceVertex[] faceVertices = new FaceVertex[parts.Length - 1];
                        for (int i = 1; i < parts.Length; i++)
                        {
                            string[] indices = parts[i].Split('/');
                            
                            FaceVertex fv = new FaceVertex();
                            if (indices.Length > 0 && int.TryParse(indices[0], out int vIdx))
                            {
                                fv.VertexIndex = vIdx - 1;
                            }
                            else
                            {
                                fv.VertexIndex = -1;
                            }

                            if (indices.Length > 1 && int.TryParse(indices[1], out int tIdx))
                            {
                                fv.TextureIndex = tIdx - 1;
                            }
                            else
                            {
                                fv.TextureIndex = -1;
                            }

                            if (indices.Length > 2 && int.TryParse(indices[2], out int nIdx))
                            {
                                fv.NormalIndex = nIdx - 1;
                            }
                            else
                            {
                                fv.NormalIndex = -1;
                            }

                            faceVertices[i - 1] = fv;
                        }
                        faces.Add(faceVertices);
                        break;
                }
            }
        }

        public void Draw()
        {
            GL.PushMatrix();

            GL.Translate(Position);
            GL.Scale(Size, Size, Size);

            GL.Rotate(Rotation.X, 1, 0, 0);
            GL.Rotate(Rotation.Y, 0, 1, 0);
            GL.Rotate(Rotation.Z, 0, 0, 1);

            if (textureId > 0)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, textureId);
                GL.Color3(Color.White);
            }
            else
            {
                GL.Disable(EnableCap.Texture2D);
                GL.Color3(color);
            }

            GL.Begin(PrimitiveType.Triangles);
            foreach (var face in faces)
            {
                for (int i = 1; i < face.Length - 1; i++)
                {
                    DrawVertex(face[0]);
                    DrawVertex(face[i]);
                    DrawVertex(face[i + 1]);
                }
            }
            GL.End();
            GL.PopMatrix();
        }

        private void DrawVertex(FaceVertex fv)
        {
            if (fv.NormalIndex >= 0 && fv.NormalIndex < normals.Count)
            {
                GL.Normal3(normals[fv.NormalIndex]);
            }

            if (fv.TextureIndex >= 0 && fv.TextureIndex < textureCoords.Count)
            {
                GL.TexCoord2(textureCoords[fv.TextureIndex]);
            }
            
            if (fv.VertexIndex >= 0 && fv.VertexIndex < vertices.Count)
            {
                GL.Vertex3(vertices[fv.VertexIndex]);
            }
        }
    }
}
