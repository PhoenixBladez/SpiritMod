using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class CoilSummonStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Rod");
			Tooltip.SetDefault("Summons a stationary electric turret\nThis turret shoots beams of lightning that jump from enemy to enemy");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Summon/CoilSummonStaff_Glow");
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.08f, .4f, .28f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Summon/CoilSummonStaff_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}////

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.QueenSpiderStaff);
			item.damage = 19;
			item.mana = 12;
			item.width = 40;
			item.height = 40;
			item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Green;
			item.knockBack = 2.5f;
			item.UseSound = SoundID.Item25;
			item.shoot = ModContent.ProjectileType<CoilSentrySummon>();
			item.shootSpeed = 0f;
		}

		public override bool CanUseItem(Player player)
		{
			player.FindSentryRestingSpot(item.shoot, out int worldX, out int worldY, out _);
			worldX /= 16;
			worldY /= 16;
			worldY--;
			return !WorldGen.SolidTile(worldX, worldY);
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			Projectile.NewProjectile(worldX, worldY - pushYUp, speedX, speedY, type, damage, knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<TechDrive>(), 6);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
	}
}
