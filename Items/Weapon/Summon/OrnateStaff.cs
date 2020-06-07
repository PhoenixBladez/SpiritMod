using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class OrnateStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Staff of the Insect Brood");
			Tooltip.SetDefault("Summons swarms of tiny beetles to fight for you\nBeetles consume 1/4 of a minion slot and dissipate quickly");
		}


		public override void SetDefaults()
		{
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 1, 27, 46);
            item.rare = 1;
            item.mana = 11;
            item.damage = 8;
            item.knockBack = 2;
            item.useStyle = ItemUseStyleID.SwingThrow; 
            item.useTime = 30;
            item.useAnimation = 30; 
            item.summon = true;          
            item.shoot = ModContent.ProjectileType<BeetleMinion>();
            item.UseSound = SoundID.Item44;
        }
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        
        public override bool UseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }		
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			
            //projectile spawns at mouse cursor
            Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = value18;
            for (int i = 0; i <= Main.rand.Next(1,3); i++)
            {
                 Terraria.Projectile.NewProjectile(position.X + Main.rand.Next(-20, 20), position.Y, 0f, 0f, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
}