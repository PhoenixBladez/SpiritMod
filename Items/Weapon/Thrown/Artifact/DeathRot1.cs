using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace SpiritMod.Items.Weapon.Thrown.Artifact
{
	public class DeathRot1 : ModItem
    {
        int charger;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Rot");
            Tooltip.SetDefault("'Ancient venom courses along the blade'\nHit enemies are afflicted by 'Pestilence,' which spreads to nearby enemies\nEvery fifth throw of the weapon leaves behind multiple clouds of Plague Miasma");

        }


        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 44;
            item.height = 44;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item100;
            item.melee = true;
            item.crit = 2;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("DeathRot1Proj");
            item.useAnimation = 22;
            item.consumable = true;
            item.useTime = 22;
            item.shootSpeed = 10f;
            item.damage = 23;
            item.knockBack = 2.0f;
            item.value = Item.sellPrice(0, 4, 0, 50);
            item.rare = 2;
            item.autoReuse = true;
            item.maxStack = 1;
            item.consumable = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger >= 5)
            {
                for (int I = 0; I < 2; I++)
                {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("Miasma"), damage, knockBack, player.whoAmI, 0f, 0f);
                }
                charger = 0;
            }
            return true;
        }
    }
}
