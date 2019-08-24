using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class FrostGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		public const int LIMIT = 5;
		public const int COOLDOWN = 3;
		public const float TURNRATE = (float)(0.4 * Math.PI / 30d);
		public const float OFFSET = 50;


		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Frost;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0xee853a };
		public override string Effect => "Stinging Cold";
		public override string Addendum =>
			"+5% Movement speed\n"+
			"Critical strikes conjure Ice Spikes that orbit you\n"+
			"Every Spike beyond the fifth will be shot towards the cursor";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Glyph");
			Tooltip.SetDefault(
				"+5% Movement speed\n"+
				"Critical strikes conjure Ice Spikes that orbit you\n"+
				"Every Spike beyond the fifth will be shot towards the cursor");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 2;

			item.maxStack = 999;
		}


		public static void CreateIceSpikes(Player player, NPC target, bool crit)
		{
			if (!crit || player.whoAmI != Main.myPlayer || !target.CanLeech())
				return;

			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (modPlayer.frostCooldown > 0)
				return;

			int damage = (int)(25 * player.GetDamageBoost());
			int spikes = modPlayer.frostCount;
			if (spikes >= LIMIT)
			{
				Vector2 velocity = Main.MouseWorld - player.MountedCenter;
				float length = velocity.Length();
				length = 1 / length;
				if (float.IsNaN(length))
					velocity = new Vector2(player.direction > 0 ? 1 : -1, 0);
				else
					velocity *= length;
				velocity *= 5;
				Projectile.NewProjectileDirect(player.MountedCenter, velocity, Projectiles.FrostSpike._type,
				damage, 2f, player.whoAmI, -1);
				modPlayer.frostCooldown = 3 * COOLDOWN;
				return;
			}

			float sector = MathHelper.TwoPi / (spikes + 1);
			float rotation = modPlayer.frostRotation + spikes * sector;
			Projectile.NewProjectileDirect(player.Center, Vector2.Zero, Projectiles.FrostSpike._type,
				damage, 2f, player.whoAmI, spikes)
				.rotation = rotation;

			modPlayer.frostCount++;
			modPlayer.frostCooldown = COOLDOWN;
		}

		public static void UpdateIceSpikes(Player player)
		{
			int owner = player.whoAmI;
			int type = Projectiles.FrostSpike._type;
			int tally = 0;
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (!projectile.active || projectile.owner != owner)
					continue;
				if (projectile.type != type)
					continue;
				if (projectile.ai[1] != 0)
					continue;

				if (tally == 0)
					player.GetModPlayer<MyPlayer>().frostRotation = projectile.rotation;
				projectile.ai[0] = tally++;
			}
		}
	}
}