package configurationtool.model;

import org.json.JSONException;
import org.json.JSONObject;

/**
 *
 * @author halzate93
 */
public class Configuration {
    public static final DifficultyEnum[] difficultyEnums = {DifficultyEnum.Easy,DifficultyEnum.Medium, DifficultyEnum.Hard, DifficultyEnum.Extreme};
    private Difficulty[] difficulties;

    public Configuration(JSONObject jsonObject)throws JSONException {
        difficulties = new Difficulty[difficultyEnums.length];
        for (int i = 0; i < difficultyEnums.length; i++) {
            difficulties[i] = new Difficulty(difficultyEnums[i], jsonObject.getJSONObject(difficultyEnums[i].toString()));
        }
    }

    public Configuration(Difficulty[] difficulties) {
        this.difficulties = difficulties;
    }

    public Configuration() {
        difficulties = new Difficulty[difficultyEnums.length];
        for(int i = 0; i < difficulties.length; i++){
            difficulties[i] = new Difficulty(difficultyEnums[i]);
        }
    }
    
    public JSONObject getJsonObject(){
        JSONObject jsonObject = new JSONObject();
        for(Difficulty d : difficulties){
            jsonObject.put(d.getLevel().toString(), d.getJsonObject());
        }
        //System.out.println(jsonObject.toString());
        return jsonObject;
    }

    public Difficulty[] getDifficulties() {
        return difficulties;
    }
}
