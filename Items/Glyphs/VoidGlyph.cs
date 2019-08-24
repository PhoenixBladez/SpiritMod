using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Items.Glyphs
{
	public class VoidGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		public const int DELAY = 100;
		public const int DECAY = 3;


		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Void;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0xff057a };
		public override string Effect => "Shadow Maelstrom";
		public override string Addendum =>
			"+8% damage reduction\n"+
			"Nearby enemies will be consumed by Devouring Void\n"+
			"This effect will grow in intensity over time";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Glyph");
			Tooltip.SetDefault(
				"+8% damage reduction\n"+
				"Nearby enemies will be consumed by Devouring Void\n"+
				"This effect will grow in intensity over time");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 6;

			item.maxStack = 999;
		}


		public static void DevouringVoid(Player player)
		{
			float range = 22 * 16;
			range *= range;
			Vector2 pos = player.Center;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (!npc.active || npc.lifeMax <= 5 || npc.friendly || npc.dontTakeDamage)
					continue;
				if (Vector2.DistanceSquared(npc.Center, pos) > range)
					continue;
				GNPC npcData = npc.GetGlobalNPC<GNPC>();
				npcData.voidInfluence = true;
				if (npcData.voidStacks < 4 * DELAY)
					npcData.voidStacks++;
				npc.AddBuff(Buffs.Glyph.DevouringVoid._type, 2, true);
			}
		}

		public static void CollapsingVoid(Player player, Entity target, int damage)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (player.whoAmI == Main.myPlayer && modPlayer.voidStacks > 1 && Main.rand.Next(14) == 0)
			{
				Vector2 vel = Vector2.UnitY.RotatedByRandom(Math.PI * 2);
				vel *= (float)Main.rand.NextDouble() * 3f;
				Projectile.NewProjectile(target.Center, vel, Projectiles.VoidStar._type, damage >> 1, 0, Main.myPlayer);
			}

			if (Main.rand.Next(10) == 1)
				player.AddBuff(Buffs.Glyph.CollapsingVoid._type, 299);
		}
	}
}