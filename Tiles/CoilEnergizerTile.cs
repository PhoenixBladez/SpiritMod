using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Placeable;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles
{
    public class CoilEnergizerTile : ModTile
    {
        public override void SetDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            animationFrameHeight = 36;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Coiled Energizer");
            AddMapEntry(new Color(50, 70, 150), name);
            disableSmartCursor = true;
            dustType = 226;
        }


        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            r = .12f;
            g = .3f;
            b = 0.5f;

        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if(Main.drawToScreen) {
                zero = Vector2.Zero;
            }
            int height = tile.frameY == 36 ? 18 : 16;
            Main.spriteBatch.Draw(mod.GetTexture("Tiles/CoilEnergizerTile_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Tile t = Main.tile[i, j];
            if(t.frameX % 54 == 0 && t.frameY == 0) {
                Main.spriteBatch.Draw(Main.extraTexture[60], new Vector2(i * 16 - (int)Main.screenPosition.X - 44, j * 16 - (int)Main.screenPosition.Y - 48) + zero, null, new Color(3, 169, 252, 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            if(tile.frameX == 18 && tile.frameY == 18) {
                DoDustEffect(new Vector2(i * 16f + 8, j * 16f + 8), 74f);
            }
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) {
            offsetY = 2;
        }
        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<CoilEnergizerItem>());
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 4));
        }

        public override void NearbyEffects(int i, int j, bool closer) {
            if(closer) {
                Player player = Main.LocalPlayer;
                float speed = player.velocity.Length();
                if(speed < 8f && Main.rand.Next(7) == 0) {
                    DoDustEffect(player.MountedCenter, 46f - speed * 4.5f, 1.08f - speed * 0.13f, 2.08f - speed * 0.24f, player);
                }
                player.AddBuff(ModContent.BuffType<OverDrive>(), 60);
            }
        }

        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null) {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, 226);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .6f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }
    }
}