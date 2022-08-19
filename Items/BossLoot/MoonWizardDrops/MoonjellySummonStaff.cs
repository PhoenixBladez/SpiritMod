using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon.MoonjellySummon;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.BossLoot.MoonWizardDrops
{
	public class MoonjellySummonStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunazoa Staff");
			Tooltip.SetDefault("Summons a Moonlight Preserver\nMoonlight Preservers summon smaller jellyfish that explode\nOnly one Moonlight Preserver can exist at once\nUsing the staff multiple times takes up summon slots, but increases jellyfish spawn rates");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/BossLoot/MoonWizardDrops/MoonjellySummonStaff_Glow");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 2, 30, 0);
			Item.rare = ItemRarityID.Green;
			Item.mana = 10;
			Item.damage = 16;
			Item.knockBack = 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<MoonjellySummon>();
			Item.UseSound = SoundID.Item44;
		}
		public override bool AltFunctionUse(Player player) => true;

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
				player.MinionNPCTargetAim(true);
			else
			{
				for (int i = 0; i < 1000; ++i)
				{
					if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
					{
						Main.projectile[i].minionSlots += 1f;
						Main.projectile[i].scale += .1f;
					}
				}
			}
			return null;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for (int i = 0; i < 1000; ++i)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
					return false;
			}
			player.AddBuff(ModContent.BuffType<MoonjellySummonBuff>(), 3600);
			return player.altFunctionUse != 2;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse != 2)
			{
				for (int i = 0; i < 1000; ++i)
				{
					if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
					{
						Main.projectile[i].minionSlots += 1f;
						if (Main.projectile[i].scale < 1.3f)
							Main.projectile[i].scale += .062f;
					}
				}
			}
			return true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.08f, .4f, .28f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.Assets.Request<Texture2D>("Items/BossLoot/MoonWizardDrops/MoonjellySummonStaff_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}