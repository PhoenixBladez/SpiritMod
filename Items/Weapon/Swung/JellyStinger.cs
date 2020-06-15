using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Sword;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class JellyStinger : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Jelly Stinger");
        }


       // int charger;
        public override void SetDefaults() {
            item.damage = 25;
            item.melee = true;
            item.width = 1;
            item.height = 1;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<JellyStingerProj>();
            item.shootSpeed = 10f;
            item.crit = 8;
            item.autoReuse = false;
             item.noUseGraphic = true;
        }
    }
}

