
//Texture
uniform int numberOfRows;
uniform vec2 Offset;

out vec2 textureCoor2;

vec2 GetTextureCoordinate()
{
	return (aTextureCoor/numberOfRows) + Offset;
}