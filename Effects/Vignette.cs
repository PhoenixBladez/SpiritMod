using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

public class Vignette : ScreenShaderData
{
	public Vignette(Effect effect, string pass) : base(new Ref<Effect>(effect), pass)
	{

	}
}