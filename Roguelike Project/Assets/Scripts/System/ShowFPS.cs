using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    [SerializeField, Tooltip("Updates per Second")]
    private float updateRate = 1.0f;

    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;

    [SerializeField]
    private float fpsWarningLimit = 30.0f;
    [SerializeField]
    private float fpsDangerLimit = 25.0f;

    private GUIStyle dangerStyle;
    private GUIStyle warningStyle;
    private GUIStyle normalStyle;

    public static bool isVisible = false;


    private void Start()
    {
        dangerStyle = new GUIStyle()
        { fontSize = 90 };
        warningStyle = new GUIStyle()
        { fontSize = 60 };
        normalStyle = new GUIStyle()
        { fontSize = 30 };
        dangerStyle.normal.textColor = Color.red;
        warningStyle.normal.textColor = Color.yellow;
        normalStyle.normal.textColor = Color.white;

        isVisible = true;
    }

    private void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
    }

    private void OnGUI()
    {
        if (!isVisible)
        {
            return;
        }

        GUIStyle style = fps <= fpsDangerLimit ? dangerStyle : (fps <= fpsWarningLimit ? warningStyle : normalStyle);
        GUI.Label(new Rect(8, 250, 70, 32), string.Format("FPS: {0:0.00}", fps), style);

        // Showing the application version
        // Using flexible display to find the lower rigth position of the screen
        GUILayout.BeginArea(new Rect(Screen.width - 150, 0, Screen.width, Screen.height));
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        //GUILayout.Label(string.Format("Build version: {0}", Application.version.ToString()), GUILayout.Width(130), GUILayout.Height(50));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}