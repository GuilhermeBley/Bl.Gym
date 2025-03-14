import { View, Text, Button, TextInput, FlatList, TouchableOpacity } from 'react-native';
import { FormikProps, FieldArray } from 'formik';
import FilteredInputSelect from '../../FilteredInputSelect';
import StyledInputFormik from '../../StyledInputFormik';
import Divider from '../../Divider';
import colors from '../../../styles/colors';
import FixedSizeModal from '../../FixedSizeModal';
import styles from './styles';
import { useState } from 'react';

interface TrainingInfo {
  id: string
  name: string
}

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

interface PageData {
  filter: string | undefined,
  selectedItem: TrainingInfo | undefined,
  availableTrainings: TrainingInfo[],
  isModalOpen: boolean,
}

const CreateOrEditSectionComponent: React.FC<CreateOrEditSectionComponentProps> = ({
  trainingIndex,
  section,
  formikProps,
  trainingData = new Map<string, string>(),
  onLoadingMoreTrainingData = () => { }
}) => {

  const [componentData, setComponentData] = useState<PageData>({
    availableTrainings: trainingData.entries().map(e => ({ id: e[0], name: e[1]})).toArray(),
    filter: undefined,
    selectedItem: undefined,
    isModalOpen: false
  });

  const filteredData =
    componentData.selectedItem
    ? componentData.availableTrainings
    : componentData.availableTrainings.filter(item =>
      item.name.toLowerCase().includes((componentData.selectedItem?.name + '').toLowerCase())
    );

  const handleExerciseSelected = () => {

  }

  if (trainingData.size == 0) return (
    <View>
      <Text style={{textAlign: "center", backgroundColor: colors.warning}}>Nenhum treino encontrado.</Text>
    </View>
  );

  let sections = formikProps.values.sections as TrainingCreationModel[] ?? [];

  section = section ?? ({ muscularGroup: '', sets: [] });

  return (
    <View>

      <FixedSizeModal visible={false} >
        <View style={styles.modalContent}>
          <TextInput
            style={styles.input}
            placeholder="Filter items..."
            value={componentData.filter}
            onChangeText={text => setComponentData(prev => ({
              ...prev,
              filter: text
            }))}
          />
          <FlatList
            data={filteredData}
            keyExtractor={(item) => item.id}
            renderItem={({ item }) => (
              <TouchableOpacity
                style={[
                  styles.item,
                  componentData.selectedItem?.id === item.id && styles.selectedItem,
                ]}
                onPress={() => setComponentData(prev => ({
                  ...prev,
                  selectedItem: item
                }))}
              >
                <Text>{item.name}</Text>
              </TouchableOpacity>
            )}
          />
          <Button
            title="Select"
            onPress={handleExerciseSelected}
            disabled={!componentData.selectedItem}
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