using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books
{
    class Book_LuminousArt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Glowing Ocean!");
            Tooltip.SetDefault("By an unknown explorer\nIt seems to be a sketch of some strange ocurrance at coastal regions");
        }
        public override void SetDefaults()
        {
            Item.noMelee = true;
            Item.useTurn = true;
            //item.channel = true; //Channel so that you can held the weapon [Important]
            Item.rare = ItemRarityID.Green;
            Item.width = 54;
            Item.height = 50;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.noUseGraphic = false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.LocalPlayer.whoAmI) return true;

			if (ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == Item.Name)
            {
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.UILuminousOceanArtState());
            }
            return null;
        }
    }
}