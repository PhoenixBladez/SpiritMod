using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SwordsMisc.HolySword
{
	public class HolySword : ModItem
	{
		int charger;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Sword");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/SwordsMisc/HolySword/HolySword_Glow");
		}

		public override void SetDefaults()
		{
			item.damage = 84;
			item.useTime = 16;
			item.useAnimation = 16;
			item.melee = true;
			item.width = 60;
			item.height = 64;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 9;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item70;
			item.autoReuse = true;
			item.useTurn = true;
		}

		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
			hitbox.Width = 20 * charger;
			hitbox.Height = 20 * charger;
			hitbox.X = (int)player.Center.X - 50 * player.direction;
			if (player.gravDir > 0) { hitbox.Y = (int)player.Bottom.Y + 16 - hitbox.Height; }
			else { hitbox.Y = (int)player.Top.Y - 16; }
			if (player.direction < 0) hitbox.X -= hitbox.Width;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
			for (int i = 0; i < 1; i++)
			{
				float length = (hitbox.Width * 1.2f - i * hitbox.Width / 18) * item.scale - 40;
				int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, DustID.PurpleCrystalShard, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 0, new Color(), 0.7f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
			for (int i = 0; i < 1; i++)
			{
				float length = (item.width * 1.2f - i * item.width / 18) * item.scale;
				int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, DustID.PurpleCrystalShard, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 0, new Color(), 0.7f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void UseStyle(Player player)
		{
			if (charger < 6)
				charger++;
		}
	}
}