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
  availableTrainings: TrainingInfo[]
}

const CreateOrEditSectionComponent: React.FC<CreateOrEditSectionComponentProps> = ({
  trainingIndex,
  section,
  formikProps,
  trainingData = new Map<string, string>(),
  onLoadingMoreTrainingData = () => { }
}) => {

  const [pageData, setPageData] = useState<PageData>({
    availableTrainings: [],
    filter: undefined,
    selectedItem: undefined
  });

  const filteredData =
    pageData.selectedItem
    ? pageData.availableTrainings
    : pageData.availableTrainings.filter(item =>
      item.name.toLowerCase().includes((pageData.selectedItem?.name + '').toLowerCase())
    );

  const handleExerciseSelected = () => {

  }

  let sections = formikProps.values.sections as TrainingCreationModel[] ?? [];

  section = section ?? ({ muscularGroup: '', sets: [] });

  return (
    <View>

      <FixedSizeModal visible={false} >
        <View style={styles.modalContent}>
          <TextInput
            style={styles.input}
            placeholder="Filter items..."
            value={pageData.filter}
            onChangeText={text => setPageData(prev => ({
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
                  pageData.selectedItem?.id === item.id && styles.selectedItem,
                ]}
                onPress={() => setPageData(prev => ({
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
            disabled={!pageData.selectedItem}
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