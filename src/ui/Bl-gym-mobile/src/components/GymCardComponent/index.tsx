import { View, Text } from "react-native";
import { styles } from "./styles";


export interface GymCardInfo{
    id: string,
    name: string,
    description: string,
    createdAt: Date,
    role: string,
};

const GymCardComponent = (item: GymCardInfo) => {
    return (
        <View style={styles.card}>
            <View
                style={styles.cardContent}>
                <Text style={styles.cardTitle}>
                    {item.name}
                    <Text style={styles.cardText}>
                        ({translateGymRoleGroup(item.role)})
                    </Text>
                </Text>
                <Text style={styles.cardText}>
                    {item.description ?? "Nenhuma descrição adicionada..."}
                </Text>
            </View>
        </View>
    );
}

function translateGymRoleGroup(roleName: string) {
    let entry = gymRoleGroupTranslations.find(item => {
        const itemKey = Object.keys(item)[0];
        return itemKey.toLowerCase() === roleName.toLowerCase();
    });

    return entry ? Object.values(entry)[0] : undefined;
}

export default GymCardComponent;