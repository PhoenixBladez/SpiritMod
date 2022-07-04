﻿using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Books.MaterialPages
{
    class MarblePage : MaterialPage
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notes on Ancient Marble Chunks");
            Tooltip.SetDefault("by Professor Alex Tannis\nContains information on a strange ore found in Marble Caverns");
        }
        public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.LocalPlayer.whoAmI) return true;

			if (!(ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == Item.Name))
			{
                SoundEngine.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.MaterialUI.UIMarbleMaterialPageState());
            }
            return null;
        }
    }

    class EnchantedLeafPage : MaterialPage
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notes on Enchanted Leaves");
            Tooltip.SetDefault("by Professor Alex Tannis\nContains information on mystical leaves found in the Briar");
        }
        public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.LocalPlayer.whoAmI) return false;

			if (!(ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == Item.Name))
			{
                SoundEngine.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.MaterialUI.UIEnchantedLeafPageState());
            }
            return null;
        }
    }

    class GranitePage : MaterialPage
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notes on Enchanted Granite Chunks");
            Tooltip.SetDefault("by Professor Alex Tannis\nContains information on a strange ore found in Granite Caverns");
        }
        public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.LocalPlayer.whoAmI) return false;

			if (!(ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == Item.Name))
			{
                SoundEngine.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.MaterialUI.UIGraniteMaterialPageState());
            }
            return null;
        }
    }

    class HeartScalePage : MaterialPage
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notes on Heart Scales");
            Tooltip.SetDefault("by Professor Alex Tannis\nContains information on a glimmering scale often found near Ardorfish");
        }
        public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.LocalPlayer.whoAmI) return false;

			if (!(ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == Item.Name))
			{
                SoundEngine.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.MaterialUI.UIHeartScalePageState());
            }
            return null;
        }
    }

    class BismitePage : MaterialPage
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notes on Bismite Crystals");
            Tooltip.SetDefault("by Professor Alex Tannis\nContains information on a toxic ore fond around the caverns");
        }
        public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.LocalPlayer.whoAmI) return false;

			if (!(ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == Item.Name))
			{
                SoundEngine.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.MaterialUI.UIBismitePageStsate());
            }
            return null;
        }
    }

    class GlowrootPage : MaterialPage
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notes on Glowroot");
            Tooltip.SetDefault("by Professor Alex Tannis\nContains information on a strange root found at the base of tall mushroom trees");
        }
        public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.LocalPlayer.whoAmI) return false;

			if (!(ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == Item.Name))
			{
                SoundEngine.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.MaterialUI.UIGlowrootPageState());
            }
            return null;
        }
    }

    class FrigidFragmentPage : MaterialPage
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Notes on Frigid Fragments");
            Tooltip.SetDefault("by Professor Alex Tannis\nContains information on an icy crystal found in the frozen tundra");
        }
        public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.LocalPlayer.whoAmI)
				return true;

            if (!(ModContent.GetInstance<SpiritMod>().BookUserInterface.CurrentState is UI.UIBookState currentBookState && currentBookState.title == Item.Name))
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<SpiritMod>().BookUserInterface.SetState(new UI.MaterialUI.UIFrigidFragmentPageState());
            }
            return null;
        }
    }
}
