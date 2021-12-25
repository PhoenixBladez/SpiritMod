using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Gores
{
	public class GamblerCash : ModGore
	{
		public override void OnSpawn(Gore gore)
		{
			gore.velocity = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() * MathHelper.TwoPi);
			updateType = 910;
		}
	}
}