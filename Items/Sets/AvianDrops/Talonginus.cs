using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.AvianDrops
{
	public class Talonginus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talonginus");
			Tooltip.SetDefault("Extremely quick, but innacurate");
		}


		int currentHit;
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 30, 0);
			Item.rare = ItemRarityID.Green;
			Item.crit = 3;
			Item.damage = 21;
			Item.knockBack = 2.5f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 9;
			Item.useAnimation = 9;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<TalonginusProj>();
			Item.shootSpeed = 9f;
			Item.UseSound = SoundID.Item1;
			this.currentHit = 0;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[Item.shoot] > 0)
				return false;
			return true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			//create velocity vectors for the two angled projectiles (outwards at PI/15 radians)
			Vector2 origVect = new Vector2(speedX, speedY);
			Vector2 newVect = Vector2.Zero;
			if (Main.rand.Next(2) == 1) {
				newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
			}
			else {
				newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
			}
			speedX = newVect.X;
			speedY = newVect.Y;
			this.currentHit++;
			return true;
		}
	}
}