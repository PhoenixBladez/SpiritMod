using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class HeartilleryBeacon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aching Heart");
			Tooltip.SetDefault("Summons a stationary heartillery that shoots blood at foes");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.QueenSpiderStaff);
			item.damage = 22;
			item.mana = 11;
			item.width = 20;
			item.height = 30;
			item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
			item.rare = ItemRarityID.Green;
			item.knockBack = 2.5f;
			item.UseSound = SoundID.Item25;
			item.shoot = ModContent.ProjectileType<HeartilleryMinion>();
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
	}
}
