using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

public class GlitchScreenShader : ScreenShaderData
{
	public GlitchScreenShader(Effect effect) : base(new Ref<Effect>(effect), "Pass1")
	{

	}
}