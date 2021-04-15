using UnityEngine;
using UnityEngine.UI;

public class TD_Lives : MonoBehaviour
{
    private TD_Game game_;
    [SerializeField] private Text lives_text_;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        game_ = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TD_Game>();
    }

    // Update is called once per frame
    void Update()
    {
        lives_text_.text = game_.player_lives_.ToString();
    }
}
