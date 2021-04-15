using UnityEngine;
using UnityEngine.UI;

public class TD_Score : MonoBehaviour
{
    private TD_Game game_;
    [SerializeField] private Text currency_text_;
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
        currency_text_.text = game_.money_.ToString();
    }
}
