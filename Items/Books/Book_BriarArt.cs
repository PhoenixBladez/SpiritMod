using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_BriarArt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flora of the Briar");
            Tooltip.SetDefault("by Field Researcher Laywatts\nIt seems to be a page torn from a book about the Briar\nContains an intricate diagram of Briar ecology");
        }
        public override void SetDefaults()
        {
            item.noMelee = true;
            item.useTurn = true;
            //item.channel = true; //Channel so that you can held the weapon [Important]
            item.rare = ItemRarityID.Green;
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
			if (player.whoAmI != Main.LocalPlayer.whoAmI) return true;

            if (ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == item.Name)
            {
            }
            else
            {
                Main.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.UIBriarArtState());
            }
            return true;
        }
    }
}