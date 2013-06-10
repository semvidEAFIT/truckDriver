package configurationtool.controller;

import configurationtool.model.Configuration;
import java.io.File;
import sun.reflect.generics.reflectiveObjects.NotImplementedException;

/**
 * file se asigna una vez se haya cargado, de esto depende el comportamiento
 * @author halzate93
 */
public class DAO {
    private File file;

    public DAO() {
    }
    
    public void saveConfiguration(Configuration configuration){
        throw new NotImplementedException();
        //TODO --- create or save file with json serialization for configuration
        //TODO --- if file == null use JFileChooser to select save file
    }
    
    public Configuration loadConfiguration(){
        //TODO --- use a  JFileChooser to select file and parse the file
        throw new NotImplementedException();
    }
}
