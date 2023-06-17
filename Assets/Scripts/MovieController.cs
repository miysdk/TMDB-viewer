using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class MovieController : MonoBehaviour
{
    public Transform MoviesContainer;
    public GameObject MovieUIPrefab;
    void Start()
    {
        StartCoroutine(GetRequest("https://api.themoviedb.org/3/discover/movie?include_adult=false&include_video=false&language=en-US&page=1&sort_by=popularity.desc&api_key=00243336e2f949edba05fc655da4510e"));
    }

    IEnumerator GetRequest(string uri){
        using(UnityWebRequest request = UnityWebRequest.Get(uri)){
            yield return request.SendWebRequest();
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(string.Format("SMTH went wrong {0}", request.error));
                    break;
                case UnityWebRequest.Result.Success:
                    RootMovie movies = JsonConvert.DeserializeObject<RootMovie>(request.downloadHandler.text);
                    foreach (Movie movie in movies.results)
                    {
                        GameObject tmp = Instantiate(MovieUIPrefab, MoviesContainer);
                        MovieUI tmpui = tmp.GetComponent<MovieUI>();
                        tmpui.SetData(movie.title, movie.overview, "http://image.tmdb.org/t/p/w500/" + movie.backdrop_path);
                        Debug.Log("http://image.tmdb.org/t/p/w500/" + movie.backdrop_path);
                    }
                    break;
            }
        }
    }
}
