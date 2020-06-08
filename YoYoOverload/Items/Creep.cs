using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
    public class Creep : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("The Creeper");
        }

        public override void SetDefaults() {
            item.CloneDefaults(ItemID.WoodYoyo);
            item.damage = 15;
            item.value = Item.buyPrice(gold: 9, copper: 40);
            item.rare = 2;
            item.knockBack = 3f;
            item.channel = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 25;
            item.useTime = 25;
            item.shoot = ModContent.ProjectileType<CreepP>();
        }
    }
}
