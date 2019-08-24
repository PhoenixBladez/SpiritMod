using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
	public class NeutronStarStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Neutron Star Staff");
            Tooltip.SetDefault("~Donator Item~\nSummons a stationary Neutron Star to launch lightning at enemies");

        }


		public override void SetDefaults()
		{
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = 9;
            item.mana = 30;
            item.damage = 85;
            item.knockBack = 1;
            item.useStyle = 1;
            item.useTime = 15;
            item.useAnimation = 15;
            item.summon = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("NeutronStar");
            item.buffTime = 3600;
			item.shootSpeed = 0f;
           item.UseSound = SoundID.Item25;
        }
		
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//remove any other owned SpiritBow projectiles, just like any other sentry minion
			for(int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile p = Main.projectile[i];
				if (p.active && p.type == item.shoot && p.owner == player.whoAmI) 
				{
					p.active = false;
				}
			}
            //projectile spawns at mouse cursor
            Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = value18;
            return true;
        }
		
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ZeusLightning", 1);
            recipe.AddIngredient(3569, 1);
			 recipe.AddIngredient(null, "AccursedRelic", 6);
            recipe.AddTile(412);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}