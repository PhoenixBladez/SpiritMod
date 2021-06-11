using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.Undead_Warlock_Staff
{
	public class Undead_Warlock_Staff : ModItem
	{
		public int damageBonus = 0;
		public override void SetDefaults()
		{
			item.noUseGraphic = true;
			item.width = 26;
			item.height = 46;
			item.autoReuse = true;
			item.holdStyle = 2;
			item.rare = 1;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 100;
			item.value = Item.sellPrice(silver: 50);
			item.useTurn = false;
		}
		public override void HoldItem(Player player)
		{
			Items.Weapon.Summon.Undead_Warlock_Staff.Undead_Warlock_Staff_Visual.isHolding = true;	
		}
		public override bool UseItem(Player player)
		{
			Undead_Warlock_Staff_Visual modPlayer = (Undead_Warlock_Staff_Visual)player.GetModPlayer(mod, "Undead_Warlock_Staff_Visual");
			modPlayer.sacrificed = true;
			modPlayer.coolDown = 10*60;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.minion && projectile.active && projectile.owner == player.whoAmI && player.ownedProjectileCounts[projectile.type] > 0)
				{
					modPlayer.flag = true;
				}
			}
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Undead Warlock's Staff");
			Tooltip.SetDefault("Can be used to sacrifice your minions\nSacrificed minions will return mana and life and grant 25% increased minion damage for 10 seconds");
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(20, -20);
		}
	}
}
