namespace MbyronModsCommon;
using ColossalFramework.UI;
using MbyronModsCommon.UI;
using UnityEngine;
using ColossalFramework;

public abstract class ToolButtonBase<T> : CustomUIButton where T : SingletonConfig<T>, new() {
    public virtual Vector2 DefaultPosition { get; set; } = Vector2.zero;

    public override void Start() {
        base.Start();
        size = new Vector2(40, 40);
        ButtonType = UIButtonType.Toggle;
        bgAtlas = CustomUIAtlas.MbyronModsAtlas;
        offBgSprites.SetSprites(CustomUIAtlas.Circle);
        offBgSprites.SetColors(CustomUIColor.OPButtonNormal);
        OffBgHoveredColor = CustomUIColor.OPButtonHovered;
        onBgSprites.SetSprites(CustomUIAtlas.Circle);
        onBgSprites.SetColors(CustomUIColor.GreenFocused);
        OnBgHoveredColor = CustomUIColor.GreenHovered;
        if (GetPosition().Equals(Vector2.zero)) {
            relativePosition = DefaultPosition;
        } else {
            relativePosition = GetPosition();
        }
        SavePosition(relativePosition);
    }

    protected override void OnMouseMove(UIMouseEventParameter p) {
        base.OnMouseMove(p);
        if (p.buttons.IsFlagSet(UIMouseButton.Right)) {
            var ratio = UIView.GetAView().ratio;
            position = new Vector3(position.x + (p.moveDelta.x * ratio), position.y + (p.moveDelta.y * ratio), position.z);
            SavePosition(relativePosition.x, relativePosition.y);
        }
    }

    protected void SavePosition(float X, float Y) {
        SingletonConfig<T>.Instance.ToolButtonPositionX = X;
        SingletonConfig<T>.Instance.ToolButtonPositionY = Y;
        SingletonConfig<T>.Save();
    }

    protected void SavePosition(Vector2 vector2) {
        SingletonItem<T>.Instance.ToolButtonPositionX = vector2.x;
        SingletonItem<T>.Instance.ToolButtonPositionY = vector2.y;
        SingletonConfig<T>.Save();
    }

    protected Vector2 GetPosition() => new(SingletonItem<T>.Instance.ToolButtonPositionX, SingletonItem<T>.Instance.ToolButtonPositionY);
}
