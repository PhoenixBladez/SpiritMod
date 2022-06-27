using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BriarChestLoot
{
	public class ReachChestMagic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leafstrike Staff");
			Tooltip.SetDefault("Summons a sharp leaf that can be controlled with the cursor");
			Item.staff[Item.type] = true;
		}

		protected override bool CloneNewInstances => true;

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true;
			Item.width = 26;
			Item.height = 26;
			Item.useTime = 34;
			Item.mana = 8;
			Item.useAnimation = 34;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2f;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = false;
			Item.shootSpeed = 6;
			Item.shoot = ModContent.ProjectileType<LeafProjReachChest>();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 50f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
		}
	}
}
