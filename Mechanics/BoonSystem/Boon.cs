using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Items.Sets.OlympiumSet;
using SpiritMod.Prim;

namespace SpiritMod.Mechanics.BoonSystem
{
	public abstract class Boon
	{
		public NPC npc;
		public Texture2D Texture => ModContent.Request<Texture2D>(TexturePath, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		public virtual bool CanApply => false;

		#region boon hooks

		public virtual void SpawnIn() { }

		public virtual void SetStats() { }
		public virtual string TexturePath => "";
		public virtual Vector2 SigilSize => Vector2.Zero;

		public virtual Vector2 SigilPosition
		{
			get
			{
				//Hover up and down with sine function
				float verticalOffset = 20;
				verticalOffset *= (float)(Math.Sin(Timer * 2) * 0.33f) + 1;

				return npc.Top - new Vector2(0, SigilSize.Y / 2) - Vector2.UnitY * verticalOffset;
			}
		}

		public virtual float Timer => Main.GameUpdateCount / 40f;

		public virtual void PreDraw(SpriteBatch spriteBatch, Color lightColor) { }

		public virtual void PostDraw(SpriteBatch spriteBatch, Color lightColor) { }
		public virtual void AI() { }

		public virtual void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) { }

		public virtual void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit) { }

		public virtual void OnDeath() { }

		public virtual void Death() { }

		#endregion

		#region helpers
		protected void DrawBeam(Color lightColor, Color darkColor)
		{

			Effect effect = ModContent.Request<Effect>("Effects/EmpowermentBeam", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			effect.Parameters["uTexture"].SetValue(ModContent.Request<Texture2D>("Textures/Trails/Trail_2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
			effect.Parameters["progress"].SetValue(Main.GlobalTimeWrappedHourly / 3);
			effect.Parameters["uColor"].SetValue(darkColor.ToVector4());
			effect.Parameters["uSecondaryColor"].SetValue(lightColor.ToVector4());

			Vector2 dist = SigilPosition - npc.Bottom;

			TrianglePrimitive tri = new TrianglePrimitive()
			{
				TipPosition = SigilPosition - Main.screenPosition,
				Rotation = MathHelper.PiOver2,
				Height = 100 + dist.Length() * 1.5f,
				Color = Color.White * 0.33f,
				Width = 100 + ((npc.width + npc.height) * 0.5f)
			};

			PrimitiveRenderer.DrawPrimitiveShape(tri, effect);
		}

		protected void DrawBloom(SpriteBatch spriteBatch, Color color, float scale)
		{
			Texture2D glow = Terraria.GameContent.TextureAssets.Extra[49].Value;
			color.A = 0;

			float glowScale = 1 + ((float)Math.Sin(Timer) / 4);

			spriteBatch.Draw(glow, SigilPosition - Main.screenPosition, null,
				color * glowScale, 0, glow.Size() / 2, npc.scale * scale, SpriteEffects.None, 0f);
		}

		protected void DrawSigil(SpriteBatch spriteBatch)
		{
			float progress = Timer % 1;
			float transparency = (float)Math.Pow(1 - progress, 2);
			float scale = 1 + progress;

			float regularscale = 0.8f;
			Color color = Color.White * npc.Opacity;
			color.A = 100;
			spriteBatch.Draw(Texture, SigilPosition - Main.screenPosition, null, color * transparency, 0, Texture.Size() / 2, npc.scale * scale * regularscale, SpriteEffects.None, 0f);

			spriteBatch.Draw(Texture, SigilPosition - Main.screenPosition, null, color, 0, Texture.Size() / 2, npc.scale * regularscale, SpriteEffects.None, 0f);
		}

		protected void DropOlympium(int stack) => Item.NewItem(npc.GetSource_Death(), npc.Center, ModContent.ItemType<OlympiumToken>(), stack);
		#endregion
	}
}