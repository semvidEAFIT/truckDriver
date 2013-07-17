package configurationtool.controller;

import configurationtool.model.Configuration;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.Scanner;
import java.util.concurrent.CancellationException;
import javax.swing.JFileChooser;
import org.json.JSONException;
import org.json.JSONObject;

/**
 * file se asigna una vez se haya cargado, de esto depende el comportamiento
 *
 * @author halzate93
 */
public class DAO {

    private File file;
    private JFileChooser chooser;

    public DAO() {
        chooser = new JFileChooser();
    }

    public void saveConfiguration(Configuration configuration) throws IOException, NullPointerException, CancellationException {
        if (file == null) {
            chooser.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
            int val = chooser.showSaveDialog(null);
            if (val == JFileChooser.APPROVE_OPTION) {
                file = new File(chooser.getSelectedFile(), "playerConfiguration.txt");
            } else {
                throw new CancellationException();
            }
        }
        PrintWriter pw = new PrintWriter(file);
        pw.write(configuration.getJsonObject().toString());
        pw.close();
    }

    public Configuration loadConfiguration() throws FileNotFoundException, JSONException, NullPointerException, CancellationException {
        File file = null;
        Configuration configuration = null;
        try {
            chooser.setFileSelectionMode(JFileChooser.FILES_ONLY);
            int val = chooser.showOpenDialog(null);

            if (val == JFileChooser.APPROVE_OPTION) {
                file = chooser.getSelectedFile();

                Scanner sc = new Scanner(file);
                String jsonString = "";
                while (sc.hasNext()) {
                    jsonString += sc.nextLine();
                }
                JSONObject jsonObject = new JSONObject(jsonString);
                configuration = new Configuration(jsonObject);
            } else {
                throw new CancellationException();
            }
        } catch (FileNotFoundException | JSONException | NullPointerException e) {
            throw e;
        }
        this.file = file;
        return configuration;
    }
}
