using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ReachChestMagic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leafstrike Staff");
			Tooltip.SetDefault("Summons a sharp leaf that can be controlled with the cursor");
		}
		public override bool CloneNewInstances => true;


		public override void SetDefaults()
		{
			item.damage = 10;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.magic = true;
			item.channel = true;
			item.width = 26;
			item.height = 26;
			item.useTime = 34;
			item.mana = 8;
			item.useAnimation = 34;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 2f;
			item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item8;
			item.autoReuse = false;
			item.shootSpeed = 6;
			item.shoot = ModContent.ProjectileType<LeafProjReachChest>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 50f;
			if(Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			return true;
		}
	}
}
