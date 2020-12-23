using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_Blossmoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notes on Blossmoons");
            Tooltip.SetDefault("by Calvin Reyes, Researcher\nIt contains notes about cute little Blossmoons");
        }
        public override void SetDefaults()
        {
            item.noMelee = true;
            item.useTurn = true;
            //item.channel = true; //Channel so that you can held the weapon [Important]
            item.rare = 2;
            item.width = 54;
            item.height = 50;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = false;
            item.noUseGraphic = false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool UseItem(Player player)
        {
            if (ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == item.Name)
            {
            }
            else
            {
                Main.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.UIBlosmoonPageState());
            }
            return true;
        }
    }
}