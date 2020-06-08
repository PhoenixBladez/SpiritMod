using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.IceSculpture
{
    public class IceBatSculpture : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ice Bat Sculpture");
        }


        public override void SetDefaults() {
            item.width = 30;
            item.height = 40;
            item.value = Item.sellPrice(0, 0, 15, 0);

            item.maxStack = 99;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = ModContent.TileType<IceBatDecor>();
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CreepingIce", 20);
            recipe.AddIngredient(null, "CryoliteBar", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}