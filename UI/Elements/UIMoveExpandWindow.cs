using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace SpiritMod.UI.Elements
{
    public class UIMoveExpandWindow : UIPanel
    {
        private enum ResizeFrom
        {
            TL,
            TR,
            BL,
            BR,
            Left,
            Top,
            Right,
            Bottom
        }

        public bool ForceScreenStick { get; set; }

        private bool _dragging;
        public Vector2 _dragOffset;

        private Texture2D _texture;
        private bool _canResize;
        private int _dragThickness;
        private bool _resizing;
        private bool _useBorder;
        private Rectangle _resizeStart;
        private ResizeFrom _resizeAnchor;

        public UIMoveExpandWindow(Texture2D texture = null, bool canResize = true, bool useBorder = true, int borderThickness = 5)
        {
            _texture = texture;
            _canResize = canResize;
            _dragThickness = borderThickness;
            _useBorder = useBorder;
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            if (evt.Target == this)
            {
                Rectangle outerRect = GetOuterDimensions().ToRectangle();
                Rectangle innerRect = GetOuterDimensions().ToRectangle();
                innerRect.Inflate(-_dragThickness, -_dragThickness);
                CalculatedStyle style = GetDimensions();
                Point mousePoint = Main.MouseScreen.ToPoint();

                if (_canResize && outerRect.Contains(mousePoint) && !innerRect.Contains(mousePoint))
                {
                    _resizing = true;
                    _resizeAnchor = GetAnchor();
                    _resizeStart = style.ToRectangle();
                }
                else
                {
                    _dragging = true;
                    _dragOffset = new Vector2(Main.mouseX - style.X, Main.mouseY - style.Y);
                }
            }

            base.MouseDown(evt);
        }

        private bool CanResize()
        {
            Rectangle outerRect = GetOuterDimensions().ToRectangle();
            Rectangle innerRect = GetOuterDimensions().ToRectangle();
            innerRect.Inflate(-_dragThickness, -_dragThickness);
            CalculatedStyle style = GetDimensions();
            Point mousePoint = Main.MouseScreen.ToPoint();
            return outerRect.Contains(mousePoint) && !innerRect.Contains(mousePoint);
        }

        private ResizeFrom GetAnchor()
        {
            Point mousePoint = Main.MouseScreen.ToPoint();
            Rectangle innerRect = GetOuterDimensions().ToRectangle();
            innerRect.Inflate(-_dragThickness, -_dragThickness);
            bool above = mousePoint.Y <= innerRect.Y;
            bool below = mousePoint.Y >= innerRect.Bottom;
            bool left = mousePoint.X <= innerRect.X;
            bool right = mousePoint.X >= innerRect.Right;
            int data = above.ToInt() * 1 + below.ToInt() * 2 + left.ToInt() * 4 + right.ToInt() * 8;
            switch (data)
            {
                case 1:
                    return ResizeFrom.Top;
                case 2:
                    return ResizeFrom.Bottom;
                case 4:
                    return ResizeFrom.Left;
                case 8:
                    return ResizeFrom.Right;
                case 5:
                    return ResizeFrom.TL;
                case 9:
                    return ResizeFrom.TR;
                case 6:
                    return ResizeFrom.BL;
                case 10:
                    return ResizeFrom.BR;
            }
            return ResizeFrom.TL;
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            if (evt.Target == this)
            {
                _dragging = false;
                _resizing = false;
            }

            base.MouseUp(evt);
        }

        public override void Update(GameTime gameTime)
        {
            if (ContainsPoint(new Vector2(Main.mouseX, Main.mouseY)))
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.LocalPlayer.showItemIcon = false;
                Main.ItemIconCacheUpdate(0);
            }

            // REMOVED: Used to change the cursor when resizing, disabled until we decide we need it.
            /* 
            if (!_dragging && (CanResize() || _resizing))
            {
                ResizeFrom anchor = _resizing ? _resizeAnchor : GetAnchor();
                switch (anchor)
                {
                    case ResizeFrom.TL:
                    case ResizeFrom.BR:
                        ExtraCursor.CursorOffsetX = -9;
                        ExtraCursor.CursorOffsetY = -9;
                        ExtraCursor.CursorResizeDiagonal = true;
                        break;
                    case ResizeFrom.TR:
                    case ResizeFrom.BL:
                        ExtraCursor.CursorOffsetX = -9;
                        ExtraCursor.CursorOffsetY = -9;
                        ExtraCursor.CursorResizeReverseDiagonal = true;
                        break;
                    case ResizeFrom.Left:
                    case ResizeFrom.Right:
                        ExtraCursor.CursorOffsetX = -13;
                        ExtraCursor.CursorOffsetY = -7;
                        ExtraCursor.CursorResizeHorizontal = true;
                        break;
                    case ResizeFrom.Top:
                    case ResizeFrom.Bottom:
                        ExtraCursor.CursorOffsetX = -7;
                        ExtraCursor.CursorOffsetY = -13;
                        ExtraCursor.CursorResizeVertical = true;
                        break;
                }
            }
            */

            if (_dragging)
            {
                Left.Set(Main.mouseX - _dragOffset.X, 0);
                Top.Set(Main.mouseY - _dragOffset.Y, 0);

                if (ForceScreenStick)
                {
                    Left.Set(MathHelper.Clamp(Left.Pixels, 0, Main.screenWidth - Width.Pixels), 0f);
                    Top.Set(MathHelper.Clamp(Top.Pixels, 0, Main.screenHeight - Height.Pixels), 0f);
                }
                Recalculate();
            }

            if (_resizing)
            {
                Rectangle newRect = Rectangle.Empty;
                Point diff;
                int targetWidth;
                int targetHeight;
                switch (_resizeAnchor)
                {
                    case ResizeFrom.TL:
                        diff = (Main.MouseScreen - _resizeStart.TopLeft()).ToPoint();
                        targetWidth = _resizeStart.Width - diff.X;
                        targetHeight = _resizeStart.Height - diff.Y;
                        ClampResizes(ref diff, ref targetWidth, ref targetHeight, -1, -1);
                        newRect = new Rectangle(_resizeStart.X + diff.X, _resizeStart.Y + diff.Y, targetWidth, targetHeight);
                        break;
                    case ResizeFrom.TR:
                        diff = (Main.MouseScreen - _resizeStart.TopRight()).ToPoint();
                        targetWidth = _resizeStart.Width + diff.X;
                        targetHeight = _resizeStart.Height - diff.Y;
                        ClampResizes(ref diff, ref targetWidth, ref targetHeight, 1, -1);
                        newRect = new Rectangle(_resizeStart.X, _resizeStart.Y + diff.Y, _resizeStart.Width + diff.X, _resizeStart.Height - diff.Y);
                        break;
                    case ResizeFrom.BL:
                        diff = (Main.MouseScreen - _resizeStart.BottomLeft()).ToPoint();
                        targetWidth = _resizeStart.Width - diff.X;
                        targetHeight = _resizeStart.Height + diff.Y;
                        ClampResizes(ref diff, ref targetWidth, ref targetHeight, -1, 1);
                        newRect = new Rectangle(_resizeStart.X + diff.X, _resizeStart.Y, _resizeStart.Width - diff.X, _resizeStart.Height + diff.Y);
                        break;
                    case ResizeFrom.BR:
                        diff = (Main.MouseScreen - _resizeStart.BottomRight()).ToPoint();
                        targetWidth = _resizeStart.Width + diff.X;
                        targetHeight = _resizeStart.Height + diff.Y;
                        ClampResizes(ref diff, ref targetWidth, ref targetHeight, 1, 1);
                        newRect = new Rectangle(_resizeStart.X, _resizeStart.Y, _resizeStart.Width + diff.X, _resizeStart.Height + diff.Y);
                        break;
                    case ResizeFrom.Left:
                        int leftOff = (int)(Main.MouseScreen.X - _resizeStart.Left);
                        targetWidth = _resizeStart.Width - leftOff;
                        ClampOneAxis(ref leftOff, MinWidth, MaxWidth, ref targetWidth, -1);
                        newRect = new Rectangle(_resizeStart.X + leftOff, _resizeStart.Y, targetWidth, _resizeStart.Height);
                        break;
                    case ResizeFrom.Top:
                        int topOff = (int)(Main.MouseScreen.Y - _resizeStart.Top);
                        targetHeight = _resizeStart.Height - topOff;
                        ClampOneAxis(ref topOff, MinHeight, MaxHeight, ref targetHeight, -1);
                        newRect = new Rectangle(_resizeStart.X, _resizeStart.Y + topOff, _resizeStart.Width, targetHeight);
                        break;
                    case ResizeFrom.Right:
                        int rightOff = (int)(Main.MouseScreen.X - _resizeStart.Right);
                        targetWidth = _resizeStart.Width + rightOff;
                        ClampOneAxis(ref rightOff, MinWidth, MaxWidth, ref targetWidth, 1);
                        newRect = new Rectangle(_resizeStart.X, _resizeStart.Y, targetWidth, _resizeStart.Height);
                        break;
                    case ResizeFrom.Bottom:
                        int bottomOff = (int)(Main.MouseScreen.Y - _resizeStart.Bottom);
                        targetHeight = _resizeStart.Height + bottomOff;
                        ClampOneAxis(ref bottomOff, MinHeight, MaxHeight, ref targetHeight, 1);
                        newRect = new Rectangle(_resizeStart.X, _resizeStart.Y, _resizeStart.Width, targetHeight);
                        break;
                }
                Left.Set(newRect.X, 0f);
                Top.Set(newRect.Y, 0f);
                Width.Set(newRect.Width, 0f);
                Height.Set(newRect.Height, 0f);

                Recalculate();
            }

            base.Update(gameTime);
        }

        private void ClampResizes(ref Point diff, ref int targetWidth, ref int targetHeight, int dirX, int dirY)
        {
            if (targetWidth > MaxWidth.Pixels)
            {
                diff.X -= (targetWidth - (int)MaxWidth.Pixels) * dirX;
                targetWidth = (int)MaxWidth.Pixels;
            }
            if (targetHeight > MaxHeight.Pixels)
            {
                diff.Y -= (targetHeight - (int)MaxHeight.Pixels) * dirY;
                targetHeight = (int)MaxHeight.Pixels;
            }
            if (targetWidth < MinWidth.Pixels)
            {
                diff.X += ((int)MinWidth.Pixels - targetWidth) * dirX;
                targetWidth = (int)MinWidth.Pixels;
            }
            if (targetHeight < MinHeight.Pixels)
            {
                diff.Y += ((int)MinHeight.Pixels - targetHeight) * dirY;
                targetHeight = (int)MinHeight.Pixels;
            }
        }

        private void ClampOneAxis(ref int offset, StyleDimension min, StyleDimension max, ref int target, int dir)
        {
            if (target > max.Pixels)
            {
                offset -= (target - (int)max.Pixels) * dir;
                target = (int)max.Pixels;
            }
            if (target < min.Pixels)
            {
                offset += ((int)min.Pixels - target) * dir;
                target = (int)min.Pixels;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (_texture == null) base.DrawSelf(spriteBatch);
            else
            {
                Rectangle rect = GetDimensions().ToRectangle();
                spriteBatch.Draw(_texture, rect, null, Color.White);
            }
        }
    }
}
