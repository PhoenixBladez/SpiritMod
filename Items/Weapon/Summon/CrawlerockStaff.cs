using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class CrawlerockStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crawlerock Staff");
			Tooltip.SetDefault("Summons bouncing mini crawlers to fight for you");
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.mana = 11;
			Item.damage = 13;
			Item.knockBack = 3;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.buffType = ModContent.BuffType<CrawlerockMinionBuff>();
            Item.shoot = ModContent.ProjectileType<Crawlerock>();
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

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => position = Main.MouseWorld;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)  {
			player.AddBuff(Item.buffType, 2);
			return player.altFunctionUse != 2;
		}
	}
}