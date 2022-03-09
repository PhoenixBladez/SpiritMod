using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using SpiritMod.Particles;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.DonatorVanity
{
	[AutoloadEquip(EquipType.Head)]
	public class PixelatedFireballVanity : ModItem
	{
		private readonly Color Blue = new Color(0, 114, 201);
		private readonly Color White = new Color(255, 255, 255);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pixelated Fireball's Mask");
			Tooltip.SetDefault("'Great for impersonating patrons!'");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;

			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.vanity = true;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			if (Main.rand.NextBool(3))
				ParticleHandler.SpawnParticle(new FireParticle(new Vector2(player.Center.X + Main.rand.Next(-10, 10), player.Center.Y - 15 + Main.rand.Next(-10, 10)), new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-4.5f, -2.5f)),
					Blue, White, Main.rand.NextFloat(0.15f, 0.45f), 30, delegate (Particle p)
					{
						p.Velocity *= 0.93f;
						p.Velocity.Y += .0125f;
					}));
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) => color = Color.White;

	}
}
