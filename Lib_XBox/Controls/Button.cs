using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib.Controls
{
    public class Button : BaseControl, IControl
    {
        #region Events
        public delegate void OnClick(Button button);
        public delegate void OnStartDown(Button button);
        public delegate void OnStartHover(Button button);
        public event OnClick Click;
        public event OnStartDown StartDown;
        public event OnStartHover StartHover;
        #endregion

        #region Members
        public override StringBuilder Caption
        {
            get { return m_Caption; }
            set
            {
                bool oldCapWasNull = m_Caption == null;
                m_Caption = value;
                if (oldCapWasNull)
                    CaptionLocation = CaptionLocation;
            }
        }

        public Texture2D OverlayTexture = null;

        public enum eCaptionLoc { TopLeft, Centered }

        protected Vector2 CaptionLoc;
        private eCaptionLoc m_CaptionLocation;
        public eCaptionLoc CaptionLocation
        {
            get { return m_CaptionLocation; }
            set
            {
                m_CaptionLocation = value;
                if (Caption != null)
                {
                    switch (value)
                    {
                        case eCaptionLoc.TopLeft:
                            CaptionLoc = Location;
                            break;
                        case eCaptionLoc.Centered:
                            CaptionLoc = new Vector2(Location.X + Texture.Width / 2 - Font.MeasureString(Caption).X / 2, Location.Y + Texture.Height / 2 - Font.MeasureString(Caption).Y / 2);
                            break;
                        default:
                            throw new CaseStatementMissingException();
                    }
                }
            }
        }

        private SpriteFont m_Font;
        public SpriteFont Font
        {
            get { return m_Font; }
            set
            {
                m_Font = value;
            }
        }
        public Texture2D Texture;
        public Texture2D HoverTexture = null;
        public Texture2D DownTexture = null;

        public object Tag;
        public object Tag2;

        public enum eState { None = 0, Hovering, Down, Clicked }
        public Color CaptionDrawColor = Color.White;

        private eState OldState = eState.None;

        private eState m_State = eState.None;
        public eState State
        {
            get { return m_State; }
            private set { m_State = value; }
        }

        public override bool HasFocus
        {
            get { return m_HasFocus; }
            set { m_HasFocus = value && !IsEntirelyDisabled; }
        }

        public override Vector2 Location
        {
            get { return m_Location; }
            set
            {
                m_Location = value;
                AABB = new Rectangle(value.Xi(), value.Yi(), Texture.Width, Texture.Height);
            }
        }

        public bool IsEntirelyDisabled = false;
       
        private bool m_IsEnabled = true;
        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set
            {
                m_IsEnabled = value;
                if (!value)
                    State = eState.None;
            }
        }
       
        #endregion

        #region Constructors
        public Button(Vector2 location, string texture)
        {
            Texture = Common.str2Tex(texture); // set texture before location
            Location = location;
        }

        public Button(Vector2 location, string texture, string hoverTexture, string downTexture)
        {
            Texture = Common.str2Tex(texture); // set texture before location
            Location = location;
            if (hoverTexture != null)
                HoverTexture = Common.str2Tex(hoverTexture);
            if (downTexture != null)
                DownTexture = Common.str2Tex(downTexture);
        }
        #endregion

        public virtual void Update(GameTime gameTime)
        {
            if (InputMgr.Instance.Mouse != null && IsEnabled && !IsEntirelyDisabled && IsVisible)
            {
                if (Collision.PointIsInRect(InputMgr.Instance.Mouse.Location, AABB))
                {
                    if (InputMgr.Instance.Mouse.LeftButtonIsPressed)
                    {
                        OldState = State;
                        State = eState.Clicked;
                        if (Click != null)
                            Click(this);
                    }
                    else if (InputMgr.Instance.Mouse.LeftButtonIsDown)
                    {
                        OldState = State;
                        State = eState.Down;
                        if (StartDown != null && OldState != eState.Down)
                            StartDown(this);
                    }
                    else
                    {
                        OldState = State;
                        State = eState.Hovering;
                        if (StartHover != null && OldState != eState.Hovering)
                            StartHover(this);
                    }
                }
                else
                {
                    OldState = State;
                    State = eState.None;
                }
            }
        }

        protected virtual void DrawByState(Color drawColor)
        {
            switch (State)
            {
                case eState.None:
                    ControlMgr.Instance.SpriteBatch.Draw(Texture, Location, drawColor);
                    break;
                case eState.Hovering:
                    if (HoverTexture != null)
                        ControlMgr.Instance.SpriteBatch.Draw(HoverTexture, Location, drawColor);
                    else
                        goto case eState.None;
                    break;
                case eState.Down:
                    if (DownTexture != null)
                        ControlMgr.Instance.SpriteBatch.Draw(DownTexture, Location, drawColor);
                    else
                        goto case eState.None;
                    break;
                case eState.Clicked:
                    goto case eState.None;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public virtual void Draw()
        {
            if (!IsEntirelyDisabled && IsVisible)
            {
                Color drawColor = Color.White;
                if (!IsEnabled)
                    drawColor = Color.DarkGray;

                DrawByState(drawColor);

                if (OverlayTexture != null)
                    ControlMgr.Instance.SpriteBatch.Draw(OverlayTexture, Location, drawColor);

                if (Caption != null)
                {
                    ControlMgr.Instance.SpriteBatch.DrawString(Font, Caption, CaptionLoc, CaptionDrawColor);
                }

                DrawChildControls();
            }
        }
    }
}
