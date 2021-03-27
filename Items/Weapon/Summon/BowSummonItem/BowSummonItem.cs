using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon.BowSummon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon.BowSummonItem
{
	public class BowSummonItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jinxbow");
			Tooltip.SetDefault("Summons an possessed bow to fight for you\nUses arrows for ammunition\nAutomatically converts fired arrows into the strongest ammo type in your inventory\nOnly one Jinxbow can exist at once");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 40;
			item.value = Item.sellPrice(0, 0, 55, 0);
			item.rare = 1;
			item.mana = 10;
			item.damage = 16;
			item.knockBack = 2;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<BowSummon>();
			item.UseSound = SoundID.Item44;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
        }
		
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            if (player.altFunctionUse != 2)
            {
                Vector2 mouse = Main.MouseWorld;
                float distance = Vector2.Distance(mouse, position);
                if (distance < 600f)
                {
                    Projectile.NewProjectile(mouse.X, mouse.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                }
                player.AddBuff(ModContent.BuffType<BowSummonBuff>(), 3600);
            }
            return false;
        }
	}
}