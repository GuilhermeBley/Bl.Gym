import { View, Text, Button } from 'react-native';
import { FormikProps, FieldArray } from 'formik';
import FilteredInputSelect from '../../FilteredInputSelect';
import StyledInputFormik from '../../StyledInputFormik';
import Divider from '../../Divider';
import colors from '../../../styles/colors';
import FixedSizeModal from '../../FixedSizeModal';

interface TrainingCreationModel {
  muscularGroup: string,
  sets: TrainingSetCreationModel[]
}

interface TrainingSetCreationModel {
  set: string,
  exerciseId: string
}

interface CreateOrEditSectionComponentProps {
  trainingIndex: number,
  section: TrainingCreationModel | undefined,
  formikProps: FormikProps<any>,
  trainingData?: Map<string, string>,
  onLoadingMoreTrainingData?: () => Promise<any>
}

const CreateOrEditSectionComponent: React.FC<CreateOrEditSectionComponentProps> = ({
  trainingIndex,
  section,
  formikProps,
  trainingData = new Map<string, string>(),
  onLoadingMoreTrainingData = () => { }
}) => {

  let sections = formikProps.values.sections as TrainingCreationModel[] ?? [];

  section = section ?? ({ muscularGroup: '', sets: [] });

  return (
    <View>

      <FixedSizeModal visible={false} >
        <View style={styles.modalContent}>
          <TextInput
            style={styles.input}
            placeholder="Filter items..."
            value={filter}
            onChangeText={setFilter}
          />
          <FlatList
            data={filteredData}
            keyExtractor={(item) => item.id}
            renderItem={({ item }) => (
              <TouchableOpacity
                style={[
                  styles.item,
                  selectedItem?.id === item.id && styles.selectedItem,
                ]}
                onPress={() => setSelectedItem(item)}
              >
                <Text>{item.name}</Text>
              </TouchableOpacity>
            )}
          />
          <Button
            title="Select"
            onPress={handleSelect}
            disabled={!selectedItem}
          />
        </View>
      </FixedSizeModal>

      <FieldArray
        name={`sections[${trainingIndex}].sets`}
        render={(arrayHelpers) => (
          <View>
            {(sections[trainingIndex]?.sets ?? []).map((set, setIndex) => (

              <View key={setIndex}>
                <View style={{ backgroundColor: colors.light }}>
                  <StyledInputFormik
                    formikKey={`sections[${trainingIndex}].sets[${setIndex}].set`}
                    formikProps={formikProps}
                    label={"Coloque as repetições"} />

                  <FilteredInputSelect
                    data={trainingData}
                    formikKey={`sections[${trainingIndex}].sets[${setIndex}].exerciseId`}
                    formikProps={formikProps}
                    label="Adicione um treino"
                    placeHolder="Digite um treino..."
                  />
                </View>

                <Divider></Divider>
              </View>
            ))}
            <Button
              title="+ Exercício"
              onPress={() => arrayHelpers.push({ exerciseId: "", set: "" } as TrainingSetCreationModel)}
            />
          </View>
        )}
      />


    </View>
  )
}

export default CreateOrEditSectionComponent;