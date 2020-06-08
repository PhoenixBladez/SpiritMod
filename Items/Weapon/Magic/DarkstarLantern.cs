using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class DarkstarLantern : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Darkstar Lantern");
            Tooltip.SetDefault("Creates a singularity at the cursor position that shoots out multiple void stars\nVoid stars home in on enemies and may inflict Shadowflame");

        }


        public override void SetDefaults() {
            item.damage = 55;
            item.magic = true;
            item.mana = 20;
            item.width = 66;
            item.height = 68;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.crit = 10;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item93;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<ShadowOrb>();
            item.shootSpeed = 1f;
        }
        public override void AddRecipes() {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "DuskStone", 10);
            modRecipe.AddIngredient(null, "StellarBar", 6);
            modRecipe.AddIngredient(ItemID.Ectoplasm, 6);
            modRecipe.AddTile(134);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            Terraria.Projectile.NewProjectile(mouse.X, mouse.Y, 0f, 0f, type, damage, knockBack, player.whoAmI);
            return false;
        }

    }
}