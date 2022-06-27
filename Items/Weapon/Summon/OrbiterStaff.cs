using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class OrbiterStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbiter Staff");
			Tooltip.SetDefault("Summons a mini meteor to charge at foes");
		}

		public override void SetDefaults()
		{
			Item.width = 56;
			Item.height = 62;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Orange;
			Item.mana = 14;
			Item.damage = 16;
			Item.knockBack = 3;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<Minior>();
			Item.UseSound = SoundID.Item44;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			if (player.altFunctionUse == 2) {
				player.MinionNPCTargetAim(true);
			}
			return base.UseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			player.AddBuff(ModContent.BuffType<MiniorBuff>(), 3600);
			return player.altFunctionUse != 2;
		}
	}
}