package configurationtool.model;

import org.json.JSONObject;
import sun.reflect.generics.reflectiveObjects.NotImplementedException;

/**
 *
 * @author halzate93
 */
public class Configuration {
    public static final DifficultyEnum[] difficultyEnums = {DifficultyEnum.Easy,DifficultyEnum.Medium, DifficultyEnum.Hard, DifficultyEnum.Extreme};
    private Difficulty[] difficulties;

    public Configuration(JSONObject jsonObject) {
        throw new NotImplementedException();
        //TODO --- initialize object from json parameters
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
        throw new NotImplementedException();
        //TODO --- serialize object parameters in json form
    }

    public Difficulty[] getDifficulties() {
        return difficulties;
    }
}
