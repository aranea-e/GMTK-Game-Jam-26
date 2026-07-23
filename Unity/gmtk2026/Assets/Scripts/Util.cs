using UnityEngine;
public static class Util {

    public static Vector2 mousePosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public static Vector2 mousePositionInt() {
        return new Vector2(Mathf.Floor(mousePosition().x), Mathf.Floor(mousePosition().y));
    }

    public static bool mouseInBounds(Vector2 low, Vector2 high) {
        return low.x <= mousePosition().x &&
               low.y <= mousePosition().y &&
               high.x >= mousePosition().x &&
               high.y >= mousePosition().y;
    }
    
}
