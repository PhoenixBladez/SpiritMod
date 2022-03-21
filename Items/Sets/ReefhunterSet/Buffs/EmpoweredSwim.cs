using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Buffs
{
	public class EmpoweredSwim : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Empowered Swim");
			Description.SetDefault("You feel much faster in water");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.ignoreWater = true;
			player.accFlipper = true;

			if (player.buffTime[buffIndex] > 2)
			{
				player.fullRotationOrigin = player.Size / 2f;
				player.fullRotation = player.velocity.ToRotation() + Microsoft.Xna.Framework.MathHelper.PiOver2;
			}
			else
			{
				player.fullRotation = 0f;
				player.AddBuff(ModContent.BuffType<SwimmingFatigue>(), 35 * 60);
			}
		}
	}
}
