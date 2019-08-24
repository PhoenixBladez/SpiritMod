using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class UnholyGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Unholy;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x08dd5d };
		public override string Effect => "Pestilence";
		public override string Addendum =>
			"+6 Armor Penetration\n"+
			"Critical strikes can inflict Wandering Plague\n"+
			"Afflicted will slowly lose life and release toxic clouds";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Glyph");
			Tooltip.SetDefault(
				"+6 Armor Penetration\n"+
				"Critical strikes can inflict Wandering Plague\n"+
				"Afflicted will slowly lose life and release toxic clouds");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 2;

			item.maxStack = 999;
		}


		public static void PlagueEffects(NPC target, int owner, ref int damage, bool crit)
		{
			damage += target.checkArmorPenetration(6);
			if (!crit || !target.CanLeech())
				return;
			if (Main.rand.NextDouble() < 0.5)
			{
				target.AddBuff(Buffs.Glyph.WanderingPlague._type, 360);
				target.GetGlobalNPC<NPCs.GNPC>().unholySource = owner;
			}
		}

		public static void ReleasePoisonClouds(NPC target, int time)
		{
			if (Main.netMode == 1)
				return;
			if (time % 80 != 0)
				return;
			int owner = target.GetGlobalNPC<NPCs.GNPC>().unholySource;
			if (!Main.player[owner].active)
				return;
			int max = time != 0 ? 1 : Main.hardMode ? 3 : 1;
			for (int i = 0; i < max; i++)
			{
				Vector2 vel = Vector2.UnitY.RotatedByRandom(Math.PI * 2);
				vel *= Main.rand.Next(8, 40) * .125f;
				int projectile = Projectile.NewProjectile(target.Center, vel, Projectiles.PoisonCloud._type, Main.hardMode ? 35 : 20, 0, owner, target.whoAmI);
				if (Main.netMode == 2)
				{
					Main.projectile[projectile].ai[0] = target.whoAmI;
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile);
				}
			}
		}
	}
}