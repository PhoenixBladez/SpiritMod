using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.NovaGun
{
    public class NovaGun : ModItem
    {
        public int projAmount = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stellanova Cannon");
            Tooltip.SetDefault("Fires erratic stars \nRight click to launch an explosive stellanova that draws in smaller stars \n95% chance not to consume ammo");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
            item.useAnimation = 40;
            item.useTime = 40;
            item.width = 38;
            item.height = 6;
            item.damage = 53;
            item.shoot = ModContent.ProjectileType<NovaGunProjectile>();
            item.shootSpeed = 12f;
            item.noMelee = true;
            item.useAmmo = AmmoID.FallenStar;
            item.value = Item.sellPrice(silver: 55);
            item.UseSound = SoundID.Item9;
            item.knockBack = 3f;
            item.ranged = true;
            item.rare = ItemRarityID.Pink;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 1);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool ConsumeAmmo(Player player) => Main.rand.NextBool(20);
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useAnimation = 40;
                item.useTime = 40;
                item.UseSound = SoundID.Item105;
                if (player.ownedProjectileCounts[ModContent.ProjectileType<NovaGunStar>()] > 0)
                    return false;
            }
            else
            {
                item.useAnimation = 10;
                item.useTime = 10;
                item.UseSound = SoundID.Item9;
            }
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 direction = new Vector2(speedX, speedY);
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(position, direction * 1.2f, ModContent.ProjectileType<NovaGunStar>(), damage * 2, knockBack, player.whoAmI);
            }
            else
            {
                float shootRotation = Main.rand.NextFloat(-0.1f, 0.1f);
                direction = direction.RotatedBy(shootRotation);
                position += direction * 5;
                player.itemRotation += shootRotation;
                Projectile.NewProjectile(position, direction, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "Starjinx", 10);
            recipe.AddIngredient(ItemID.FallenStar, 4);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
