using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Gores
{
	public class GamblerCash : ModGore
	{
		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.velocity = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() * MathHelper.TwoPi);
			UpdateType = 910;
		}
	}
}