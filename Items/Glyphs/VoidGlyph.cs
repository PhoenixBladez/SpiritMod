using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Glyph;
using SpiritMod.NPCs;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class VoidGlyph : GlyphBase, IGlowing
	{
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		public const int DELAY = 100;
		public const int DECAY = 3;


		Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Void;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0xff057a };
		public override string Effect => "Shadow Maelstrom";
		public override string Addendum =>
			"+8% damage reduction\n" +
			"Nearby enemies will be consumed by Devouring Void\n" +
			"This effect will grow in intensity over time";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Glyph");
			Tooltip.SetDefault(
				"+8% damage reduction\n" +
				"Nearby enemies will be consumed by Devouring Void\n" +
				"This effect will grow in intensity over time");
		}


		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightPurple;

			Item.maxStack = 999;
		}


		public static void DevouringVoid(Player player)
		{
			float range = 22 * 16;
			range *= range;
			Vector2 pos = player.Center;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (!npc.active || npc.lifeMax <= 5 || npc.friendly || npc.dontTakeDamage)
					continue;
				if (Vector2.DistanceSquared(npc.Center, pos) > range)
					continue;
				GNPC npcData = npc.GetGlobalNPC<GNPC>();
				npcData.voidInfluence = true;
				if (npcData.voidStacks < 4 * DELAY)
					npcData.voidStacks++;
				npc.AddBuff(ModContent.BuffType<DevouringVoid>(), 2, true);
			}
		}

		public static void CollapsingVoid(Player player, Entity target, int damage)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (player.whoAmI == Main.myPlayer && modPlayer.voidStacks > 1 && Main.rand.Next(14) == 0) {
				Vector2 vel = Vector2.UnitY.RotatedByRandom(Math.PI * 2);
				vel *= (float)Main.rand.NextDouble() * 3f;
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), target.Center, vel, ModContent.ProjectileType<VoidStar>(), damage >> 1, 0, Main.myPlayer);
			}

			if (Main.rand.Next(10) == 1)
				player.AddBuff(ModContent.BuffType<CollapsingVoid>(), 299);
		}
	}
}